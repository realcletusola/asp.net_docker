# use .NET SDk image to build and run the application 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env 

# set working dir for the build process 
WORKDIR /src

# copy csproj and restore a any dependencies  
COPY CrudApi.csproj ./CrudApi/
WORKDIR /src/CrudApi
RUN dotnet restore

# copy the rest of the application code
COPY . ./CrudApi

# publish the app to the /app/out directory 
RUN dotnet publish CrudApi.csproj -c Release -o /app/out 

# use the .Net 9 runtime image to run the application 
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime 

# set working directory for runtime 
WORKDIR /app 

# copy the published app from the build environment
COPY --from=build-env /app/out .

# expose the app port 
EXPOSE 80
EXPOSE 443

# start the application 
ENTRYPOINT ["dotnet", "CrudApi.dll"]

