using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core.Models.Contracts;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.ProjectFiles;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services.Editors;

[Route(template: "/api/editor")]
[ApiController]
public class EditorController : ControllerCustom
{
    private readonly ILogger<EditorController> _logger;
    private readonly IMapper _mapper;

    public EditorController(ILogger<EditorController> logger, AuthenticationService authenticationService, IMapper mapper) : base(authenticationService)
    {
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route(template: "get/dynamic-folders", Name = "GetDynamicFolders")]
    public async Task<ActionResult<IEnumerable<ResponseDirectoryInfo>>> GetDynamicFolders()
    {
        var user = await GetUser();

        if (user == null)
        {
            _logger.LogWarning(message: "User not connected");
            return StatusCodeGenerator.NotConnected();
        }

        if (!user.HaveAccess(EnumUserAccessLevel.Admin))
        {
            _logger.LogWarning(message: "User not allowed to access this resource");
            return StatusCodeGenerator.Forbidden();
        }

        var root = FileProjectManager.GetProjectDirectory(path: "Dynamic");
        var folders = root.GetDirectories();

        var res = _mapper.Map<IEnumerable<ResponseDirectoryInfo>>(source: folders);
        return Ok(value: res);
    }

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route(template: "get/files-in-dynamic-folder", Name = "GetFilesFromDynamicFolder")]
    public async Task<ActionResult<IEnumerable<ResponseFileInfo>>> GetFilesFromDynamicFolder(string dynamicFolderName)
    {
        var user = await GetUser();

        if (user == null)
        {
            return StatusCodeGenerator.NotConnected();
        }

        if (!user.HaveAccess(EnumUserAccessLevel.Admin))
        {
            return StatusCodeGenerator.Forbidden();
        }

        var root = FileProjectManager.GetProjectDirectory(path: "Dynamic");
        var folder = root.GetDirectories().FirstOrDefault(predicate: directory => directory.Name == dynamicFolderName);

        if (folder == null)
        {
            return StatusCodeGenerator.NotFound(objectName: "Folder not found");
        }

        var files = folder.GetFilesRecursively(searchPattern: "*.json");

        var res = _mapper.Map<IEnumerable<ResponseFileInfo>>(source: files);
        return Ok(value: res);
    }

    [HttpGet]
    [Route(template: "get/file-from-dynamic-folder", Name = "GetFileFromDynamicFolder")]
    public async Task<ActionResult<ResponseFileInfo>> GetFilesFromDynamicFolder(string dynamicFolderName, string fileName)
    {
        var user = await GetUser();

        if (user == null)
        {
            return StatusCodeGenerator.NotConnected();
        }

        if (!user.HaveAccess(EnumUserAccessLevel.Admin))
        {
            return StatusCodeGenerator.Forbidden();
        }

        var root = FileProjectManager.GetProjectDirectory(path: "Dynamic");
        var folder = root.GetDirectories().FirstOrDefault(predicate: directory => directory.Name == dynamicFolderName);

        if (folder == null)
        {
            return StatusCodeGenerator.NotFound(objectName: "Folder not found");
        }

        var file = folder.GetFilesRecursively(searchPattern: "*.json").SingleOrDefault(predicate: file => file.Name == fileName);
        if (file == null)
        {
            return StatusCodeGenerator.NotFound(objectName: "File not found");
        }


        var res = _mapper.Map<ResponseFileInfo>(source: file);

        //Add data
        res.Data = file.ReadAllText();

        return Ok(value: res);
    }
}