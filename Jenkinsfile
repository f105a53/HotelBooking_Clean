pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Building project'
        sh 'dotnet publish **/*.csproj --configuration Release'
      }
    }
    stage('Test') {
      steps {
        sh 'dotnet test **/*.csproj'
      }
    }
  }
}