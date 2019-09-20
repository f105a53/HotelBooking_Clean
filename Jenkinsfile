pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh 'dotnet build **/*.csproj --configuration Release'
      }
    }
    stage('Publish') {
      steps {
        sh 'dotnet Publish **/*.csproj --configuration Release'
      }
    }
    stage('Test') {
      steps {
        sh 'dotnet test \'**/*Tests/*.csproj'
      }
    }
  }
}