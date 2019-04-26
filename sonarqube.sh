#!/bin/bash

if [ "$APPVEYOR_PULL_REQUEST_NUMBER" = "" ]; then
  dotnet sonarscanner begin /o:thiagobarradas-github /k:ThiagoBarradas_jsonmasking /v:${version} /d:sonar.host.url=https://sonarcloud.io /d:sonar.login=${SONARQUBE_TOKEN} /d:sonar.cs.opencover.reportsPaths="opencover.xml" /d:sonar.branch.name=${APPVEYOR_REPO_BRANCH} 
else 
  dotnet sonarscanner begin /o:thiagobarradas-github /k:ThiagoBarradas_jsonmasking /v:${version} /d:sonar.host.url=https://sonarcloud.io /d:sonar.login=${SONARQUBE_TOKEN} /d:sonar.cs.opencover.reportsPaths="opencover.xml" /d:sonar.pullrequest.key=${APPVEYOR_PULL_REQUEST_NUMBER} /d:sonar.pullrequest.branch=${APPVEYOR_REPO_BRANCH} /d:sonar.pullrequest.base=${APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH} 
fi 