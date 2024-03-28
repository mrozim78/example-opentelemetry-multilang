cd ../src/frontend/OpenTelemetry.Example.Dotnet.Frontend
docker build -t frontend:1.0.0 .
cd ../../catalog
docker build -t catalog:1.0.0 .
cd ../ordering
set JAVA_HOME=C:\Sandbox\InteliJ\jdks\openjdk-21.0.1
gradlew publishImageToLocalRegistry