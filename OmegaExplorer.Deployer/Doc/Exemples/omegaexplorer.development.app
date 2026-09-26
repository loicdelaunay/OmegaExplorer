server {
    server_name dev.omegaexplorer.com;

    location / {
        proxy_pass http://localhost:5100;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    listen 443 ssl; # managed by Certbot
    ssl_certificate /etc/letsencrypt/live/dev.omegaexplorer.com/fullchain.pem; # managed by Certbot
    ssl_certificate_key /etc/letsencrypt/live/dev.omegaexplorer.com/privkey.pem; # managed by Certbot
    include /etc/letsencrypt/options-ssl-nginx.conf; # managed by Certbot
    ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem; # managed by Certbot

}

server {
    listen 5100;
    listen [::]:5100;
    server_name localhost;

    location / {
        root /app/development/app/wwwroot;
        try_files $uri $uri/ index.html =404;

        include /etc/nginx/mime.types;
        types {
            application/wasm wasm;
        }
        default_type application/octet-stream;
    }
}server {
    if ($host = dev.omegaexplorer.com) {
        return 301 https://$host$request_uri;
    } # managed by Certbot


    listen 80;
    server_name dev.omegaexplorer.com;
    return 404; # managed by Certbot


}