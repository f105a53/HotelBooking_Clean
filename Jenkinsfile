pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh '''dotnet build HotelBooking.Core/HotelBooking.Core.csproj --configuration Release
dotnet build HotelBooking.Infrastructure/HotelBooking.Infrastructure.csproj --configuration Release
dotnet build HotelBooking.Mvc/HotelBooking.Mvc.csproj --configuration Release
dotnet build HotelBooking.WebApi/HotelBooking.WebApi.csproj --configuration Release
dotnet build HotelBooking.UnitTests/HotelBooking.UnitTests.csproj --configuration Release
dotnet build HotelBooking.IntegrationTests/HotelBooking.IntegrationTests.csproj --configuration Release'''
      }
    }
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