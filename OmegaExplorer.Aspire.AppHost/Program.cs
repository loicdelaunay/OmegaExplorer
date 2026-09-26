IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ProjectResource> api = builder.AddProject<Projects.OmegaExplorer_Server>("api")
                 .WithEnvironment("OTEL_SERVICE_NAME", "api")
                 .WithOtlpExporter();


IResourceBuilder<ProjectResource> web = builder.AddProject<Projects.OmegaExplorer_Client>("web", launchProfileName: "Watch")
                 .WithExternalHttpEndpoints()
                 .WithReference(api)
                 .WaitFor(api);

builder.Build().Run();