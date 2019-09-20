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
        echo 'Testing'
        sh 'dotnet test '**/*Tests/*.csproj'
      }
    }
  }
}
