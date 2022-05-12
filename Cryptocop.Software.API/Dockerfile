FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Björn"

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=production

COPY . /app
WORKDIR /app/Cryptocop.Software.API
RUN dotnet restore
ENTRYPOINT [ "dotnet", "run" ]