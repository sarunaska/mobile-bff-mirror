FROM openshift.repo.sebank.se/sebshift/ubi-dotnet-60-sebshift:latest as build
USER root
WORKDIR /opt/app-root/src
COPY ["nuget.config", "."]
COPY . .
RUN dotnet restore --configfile nuget.config
RUN dotnet publish -o /opt/app-root/app

USER 1001

FROM openshift.repo.sebank.se/sebshift/ubi-dotnet-60-sebshift:latest as runtime
COPY --from=build /opt/app-root/app /opt/app-root/app
EXPOSE 8080
WORKDIR /opt/app-root/app
ENV ASPNETCORE_URLS=http://+:8080;
ENTRYPOINT ["dotnet", "MobileBff.dll"]
