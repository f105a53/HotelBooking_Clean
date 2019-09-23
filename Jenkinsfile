pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh 'dotnet restore'
        sh 'dotnet build HotelBooking.sln --configuration Release'
      }
    }
    stage('Test') {
      parallel {
        stage('Unit Test') {
          steps {
            sh 'dotnet test HotelBooking.UnitTests/HotelBooking.UnitTests.csproj'
          }
        }
        stage('Integration Tests') {
          steps {
            sh 'dotnet test HotelBooking.IntegrationTests/HotelBooking.IntegrationTests.csproj'
          }
        }
      }
    }
  }
}
