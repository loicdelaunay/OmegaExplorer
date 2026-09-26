using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services.Images.Models.Classes;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Volumes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;

// Added for System.IO.File.Exists

namespace OmegaExplorer.Server.Services._Game.Spaceships.Providers;

public class SpaceshipThumbnailProvider
{
    public const int MODULE_X_SIZE = 300;

    public const int MODULE_Y_SIZE = 200;

    // Changed key from Guid to string (checksum) and renamed for clarity
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locksByChecksum = new();

    // This list might be redundant with the new semaphore logic based on checksum
    // and file existence check. Consider removing or re-evaluating its purpose.
    public List<Guid> SpaceshipThumbnailProcessing = new();

    private async Task GenerateThumbnail(BlueprintSpaceship blueprintSpaceship, string checksum)
    {
        // Get or create a semaphore by checksum (initialCount = 1 for exclusive access)
        var semaphore = _locksByChecksum.GetOrAdd(checksum, key => new SemaphoreSlim(1, 1));

        // Wait for semaphore availability
        await semaphore.WaitAsync();

        try
        {
            var directory = VolumeManager.GetSpaceshipThumbnailDirectory();
            var filePath = directory.GetFilePath(checksum + ".png");

            // Check if the file was created by another thread while this one was waiting for the semaphore.
            if (File.Exists(filePath)) return; // File already exists, no need to regenerate.

            //if blueprint have modules not lazy loaded ignore
            if (blueprintSpaceship.Modules.Count == 0) return;

            List<ImagePositioning> imagesData = new();

            foreach (var blueprintSpaceshipModule in blueprintSpaceship.Modules)
            {
                var path = ApplicationStaticResource.GetPathDynamicSpaceshipModule(blueprintSpaceshipModule.Index
                    .ToString());

                try
                {
                    var image = await ApplicationStaticResource.GetImageAsByteAsync(path);
                    imagesData.Add(new ImagePositioning(image, blueprintSpaceshipModule.X, blueprintSpaceshipModule.Y));
                }
                catch (Exception e)
                {
                    Log.Error(e, "Failed to retrieve image for module {Module} at path {Path}",
                        blueprintSpaceshipModule.Index, path);
                }
            }

            var width = blueprintSpaceship.Modules.Max(module => module.X * MODULE_X_SIZE) + MODULE_X_SIZE;
            var height = blueprintSpaceship.Modules.Max(module => module.Y * MODULE_Y_SIZE) + MODULE_Y_SIZE;

            using Image<Rgba32> outputImage = new(width, height);

            foreach (var imageData in imagesData)
            {
                var xPosition = width - imageData.X * MODULE_X_SIZE - MODULE_X_SIZE;
                var yPosition = height - imageData.Y * MODULE_Y_SIZE - MODULE_Y_SIZE;

                // Resize the image with a Nearest Neighbor resampler to preserve the pixelated effect
                var resizedImage = imageData.Image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(MODULE_X_SIZE, MODULE_Y_SIZE),
                    Sampler = KnownResamplers.NearestNeighbor, // Nearest Neighbor for pixelated scaling
                    Mode = ResizeMode.Stretch
                }));

                outputImage.Mutate(ctx => ctx.DrawImage(resizedImage, new Point(xPosition, yPosition), 1));
            }

            // Save the final image
            await outputImage.SaveAsync(filePath);
        }
        catch (Exception e)
        {
            Log.Logger.Error("Failed to generate thumbnail : " + e, EnumLogSeverity.Error);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task<string> GetThumbnailPath(Spaceship spaceship,
        bool createIfNotExist = true)
    {
        try
        {
            var blueprint = spaceship.Blueprint;
            if (blueprint == null) throw new Exception("Spaceship blueprint is empty ?");

            var path = await GetThumbnailPath(blueprint, createIfNotExist);

            return path;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> GetThumbnailPath(BlueprintSpaceship blueprintSpaceship, bool createIfNotExist = true)
    {
        var res = "";

        foreach (var module in blueprintSpaceship.Modules) res += module.GetThumbnailIndex();

        var checksum = res.CreateSHA256();
        var directory = VolumeManager.GetSpaceshipThumbnailDirectory();

        if (createIfNotExist)
        {
            //Generate if not exist
            var exist = directory.IsFileExist(checksum + ".png");

            if (!exist) await GenerateThumbnail(blueprintSpaceship, checksum);
        }

        return checksum;
    }
}