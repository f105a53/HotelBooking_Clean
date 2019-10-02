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
            dir ('HotelBooking.UnitTests') {
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true --logger "trx;logfilename=unit.xml" --results-directory TestResults/'
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
			  mstest testResultsFile:"/var/lib/jenkins/workspace/oking_Clean_jenkins-test-reports/HotelBooking.UnitTests/TestResults/unit.trx", keepLongStdio: true
            }
          }
        }
        stage('Integration Tests') {
          steps {
            dir ('HotelBooking.IntegrationTests') {
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true --logger "trx;logfilename=integration.xml" --results-directory TestResults/'              
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
			  mstest testResultsFile:"/var/lib/jenkins/workspace/oking_Clean_jenkins-test-reports/HotelBooking.UnitTests/TestResults/integration.trx", keepLongStdio: true
            }
          }
        }
      }
    }
  }
}
