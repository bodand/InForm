FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app


COPY . .

RUN dotnet restore "InForm.Web/InForm.Web.csproj"

RUN dotnet publish "InForm.Web/InForm.Web.csproj"  -c Release -o out



FROM nginx:alpine
WORKDIR /app
EXPOSE 80


COPY --from=build-env /app/out/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

