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
            echo 'WORKSPACE'
            dir(path: 'HotelBooking.UnitTests') {
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true --logger "trx;logfilename=unit.xml" --results-directory WORKSPACE/TestResults/'
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
              mstest(testResultsFile: 'WORKSPACE/TestResults/*.trx', keepLongStdio: true)
            }

          }
        }
        stage('Integration Tests') {
          steps {
            dir(path: 'HotelBooking.IntegrationTests') {
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true'
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
            }

          }
        }
      }
    }
  }
}