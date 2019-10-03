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
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true --logger "trx;logfilename=unit.trx" --results-directory TestResults/'
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
			  mstest testResultsFile:"$WORKSPACE//HotelBooking.UnitTests/TestResults/unit.trx", keepLongStdio: true
            }

          }
        }
        stage('Integration Tests') {
          steps {
            dir ('HotelBooking.IntegrationTests') {
              sh 'dotnet test --configuration Release /p:AltCover=true  /p:AltCoverForce=true --logger "trx;logfilename=integration.trx" --results-directory TestResults/'              
              sh 'curl -s https://codecov.io/bash | bash -s - -t 4a8b2c4d-4136-470d-813f-3717be48c9aa'
			  mstest testResultsFile:"$WORKSPACE//HotelBooking.IntegrationTests/TestResults/integration.trx", keepLongStdio: true
            }

          }
        }
      }
    }
	stage('Deploy') {
		steps {
			sh 'sudo systemctl stop kestrel-Jenkins-hotelbooking-master-mvc.service'
			sh 'sudo systemctl stop kestrel-Jenkins-hotelbooking-master-api.service'
			sh 'dotnet publish HotelBooking.sln --configuration Release'
			sh 'sudo systemctl start kestrel-Jenkins-hotelbooking-master-mvc.service'
			sh 'sudo systemctl start kestrel-Jenkins-hotelbooking-master-api.service'
		}
	}
  }
}