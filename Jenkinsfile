pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh '''dotnet build HotelBooking.Core.csproj --configuration Release
dotnet build HotelBooking.Infrastructure.csproj --configuration Release
dotnet build HotelBooking.Mvc.csproj --configuration Release
dotnet build HotelBooking.WebApi.csproj --configuration Release
dotnet build HotelBooking.UnitTests.csproj --configuration Release
dotnet build HotelBooking.IntegrationTests.csproj --configuration Release'''
      }
    }
    stage('Unit Test') {
      steps {
        sh 'dotnet test HotelBooking.UnitTests.csproj'
      }
    }
    stage('Integration Tests') {
      steps {
        sh 'dotnet test HotelBooking.IntegrationTests.csproj'
      }
    }
  }
}