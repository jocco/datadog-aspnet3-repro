SET SERVICE_VERSION=0.0.3

PUSHD firstService
docker build . -t first-service
docker tag first-service jocco/datadog-repro-first-service:%SERVICE_VERSION%
docker push jocco/datadog-repro-first-service:%SERVICE_VERSION%
POPD

PUSHD secondService
docker build . -t second-service
docker tag second-service jocco/datadog-repro-second-service:%SERVICE_VERSION%
docker push jocco/datadog-repro-second-service:%SERVICE_VERSION%
POPD

