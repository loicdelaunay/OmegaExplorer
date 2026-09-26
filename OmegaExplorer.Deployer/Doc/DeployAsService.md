# create service
path : /etc/systemd/system/
name : omegaexplorer.development.api.service

content :

[Unit]
Description=Omega Explorer Development API Service

[Service]
WorkingDirectory=/app/development/api
ExecStart=/app/development/api/OmegaExplorer.API
Restart=always
RestartSec=10
SyslogIdentifier=omegaexplorer-development-api
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target

reload service :
- sudo systemctl daemon-reload

enable service : 
- sudo systemctl enable omegaexplorer.development.api

# system deploy example :

- sudo systemctl start omegaexplorer.development.api
- sudo systemctl stop omegaexplorer.development.api


# HTTPS :
- Nginx routing 
- Nginx certbot to certificate a server
