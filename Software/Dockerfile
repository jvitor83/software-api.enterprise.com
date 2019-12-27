#---------------Imagem usada para Instalação de ferramentas/bibliotecas adicionais
#FROM tjmt/dotnetcore:sdk-3.0.100 AS debug
FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS debug

RUN apt-get update && apt-get install -y default-jre
RUN dotnet tool install --global dotnet-sonarscanner --version 4.8.0
ENV PATH "$PATH:/root/.dotnet/tools/"

ENV SOLUTION_NAME=Software.sln
ENV PROJECT_NAME=Software
COPY ${SOLUTION_NAME} ./
COPY ./*/*.csproj *.dcproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore ${SOLUTION_NAME} --verbosity m
COPY . .
ENTRYPOINT ./entrypoint-debug.sh


#---------------Estágio usada para rodar os Testes, teste, sonarqube
FROM debug AS tests
ENTRYPOINT ./entrypoint-tests.sh


#---------------Estágio usada para build/publish/pack
FROM debug AS build
RUN dotnet build ${SOLUTION_NAME} --configuration Release --verbosity m
RUN dotnet publish ${SOLUTION_NAME} --configuration Release --no-build --output /app/www --verbosity m
RUN dotnet pack ${PROJECT_NAME} --configuration Release --no-build --output /app/package --verbosity m --include-symbols --include-source
ENTRYPOINT ./entrypoint.sh


#---------------Estágio usada para runtime
FROM tjmt/dotnetcore:aspnet-2.2 AS runtime
COPY --from=build /app/www /app/www
WORKDIR /app/www
ENTRYPOINT ./entrypoint.sh


#---------------Estágio usada para publicação (kubernetes/npm)
FROM tjmt/publicador:latest AS deploy
COPY . /var/release/source


FROM runtime AS final