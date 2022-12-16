//def commitHash = ""
//
//pipeline {
//  agent {
//    label 'jenkins-builder'
//  }
//  options {
//    ansiColor('xterm')
//    timestamps()
//    disableConcurrentBuilds()
//    timeout(time: 120, unit: 'MINUTES')
//    skipDefaultCheckout()
//  }
//
//  environment {
//    WORKERS = 1
//    NUMBER_OF_RETRY_FOR_TESTS = 1
//    BUILD_LABEL = buildLabel()
//    GIT_BRANCH = getBranchName()
//    DOCKER_IMAGE_NAME = "gcr.io/jenkins-160014/xacte.api"
//    DOCKER_LABEL = escapeBranchName(getDockerLabel())
//    GIT_URL = "https://github.com/petalmd/xacte.api"
//  }
//
//  stages {
//    stage('Provision build environment') {
//      steps {
//        script {
//
//          println scm.userRemoteConfigs['url'].first()
//          scmVars = checkout([
//            $class: 'GitSCM',
//            branches: scm.branches,
//            doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations,
//            extensions: scm.extensions + [[$class: 'SubmoduleOption', parentCredentials: true, disableSubmodules: false, recursiveSubmodules: false]],
//            submoduleCfg: [],
//            userRemoteConfigs: [[
//              credentialsId: 'github-clone',
//              url: scm.userRemoteConfigs['url'].first()
//            ]]
//          ])
//          commitHash = sh returnStdout: true, script: "echo -n \$(git rev-parse \"origin/$GIT_BRANCH\")"
//          provisionBuildEnvironment()
//        }
//      }
//    }
//    
//    stage('Unit tests') {
//      agent {
//        label buildLabel()
//      }
//      environment {
//        GIT_COMMIT = "${commitHash}"
//      }
//      steps {
//        script {
//          scmVars = checkout([
//            $class: 'GitSCM',
//            branches: scm.branches,
//            doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations,
//            extensions: scm.extensions + [[$class: 'SubmoduleOption', parentCredentials: true, disableSubmodules: false, recursiveSubmodules: true, trackingSubmodules: true]],
//            submoduleCfg: [],
//            userRemoteConfigs: [[
//              credentialsId: 'github-clone',
//              url: scm.userRemoteConfigs['url'].first()
//            ]]
//          ])
//          sh '''#!/bin/bash
//            set -e
//            docker build --no-cache -f bin/Dockerfile -t ${DOCKER_IMAGE_NAME}:${DOCKER_LABEL} .
//          '''
//          retry(NUMBER_OF_RETRY_FOR_TESTS) {
//            try {
//              sh '''
//                #!/bin/bash
//                mkdir reports
//                docker run -v $(pwd)/reports:/code/reports --rm ${DOCKER_IMAGE_NAME}:${DOCKER_LABEL} -u
//              '''
//            } finally {
//              stash includes: 'reports/**/*.xml', name: 'unit', useDefaultExcludes: false, allowEmpty: true
//            }
//          }
//          // Coveralls
//          if (env.CHANGE_ID) {
//            coverall_build_no = "${env.CHANGE_ID}-${env.BUILD_NUMBER}"
//            coverall_pr = "--pullRequest ${env.CHANGE_ID}"
//          } else {
//            coverall_build_no = "${env.BUILD_NUMBER}"
//            coverall_pr = ""
//          }
//          withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//            withCredentials([string(credentialsId: 'coveralls_xacte.api_token', variable: 'COVERALLS_REPO_TOKEN')]) {
//              sh """
//                #!/bin/bash
//                nslookup coveralls.io
//                docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                docker run -i --rm --entrypoint bash petalmd/dotnet-coveralls:latest -c \
//                  'export DEBIAN_FRONTEND=noninteractive && apt-get update && apt-get install dnsutils -y && nslookup coveralls.io'
//                docker run \
//                  -v \$(pwd):/code \
//                  --rm -i \
//                  petalmd/dotnet-coveralls:latest \
//                    --opencover --useRelativePaths \
//                    -i /code/reports/coverage.opencover.xml \
//                    --repoToken \$COVERALLS_REPO_TOKEN \
//                    --commitId \$GIT_COMMIT \
//                    --commitBranch \$GIT_BRANCH \
//                    --commitAuthor 'Jenkins' \
//                    --commitEmail 'jenkins@petalmd.com' \
//                    --jobId ${coverall_build_no} ${coverall_pr}
//              """
//            }
//          }
//        }
//      }
//    }
//
//    stage('SonaQube Scanner') {
//      agent {
//        label buildLabel()
//      }
//      steps {
//        catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
//          withSonarQubeEnv(installationName: 'SonarQube', envOnly: true) {
//            script {
//              checkout scm
//              if (env.CHANGE_ID) {
//                scannerExtrasArgs = "/d:sonar.pullrequest.branch=${env.CHANGE_BRANCH} /d:sonar.pullrequest.key=${env.CHANGE_ID} /d:sonar.pullrequest.base=${env.CHANGE_TARGET}"
//              } else {
//                scannerExtrasArgs = "/d:sonar.branch.name=${env.GIT_BRANCH}"
//              }
//              withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//                sh """#!/bin/bash
//                  docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                  docker run \
//                    --rm \
//                    -v \$(pwd):/usr/src \
//                    petalmd/dotnet-sonar-scanner:6.0 \
//                    bash -c "dotnet /sonar-scanner/SonarScanner.MSBuild.dll begin \
//                    /k:xacte.api \
//                    /d:sonar.host.url=\$SONAR_HOST_URL \
//                    /d:sonar.login=\$SONAR_AUTH_TOKEN \
//                    /d:sonar.verbose=false \
//                    ${scannerExtrasArgs} && \
//                    dotnet build ./XacteV2.sln && \
//                    dotnet /sonar-scanner/SonarScanner.MSBuild.dll end \
//                    /d:sonar.login=\$SONAR_AUTH_TOKEN"
//                """
//              }
//            }
//          }
//        }
//      }
//    }
//
//    stage(' ') {
//      stages {
//        stage('Build auth3_xacte_net') {
//          agent {
//            label buildLabel()
//          }
//          steps {
//            script {
//              checkout(scm).each { k,v -> env.setProperty(k, v) }
//              withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//                sh '''#!/bin/bash
//                set -e
//                docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                docker build --pull --no-cache \
//                             --build-arg BRANCH=${GIT_BRANCH} \
//                             --build-arg GIT_COMMIT=${GIT_COMMIT} \
//                             --build-arg JOB_NAME=${JOB_NAME} \
//                             --build-arg TAG_NAME=${TAG_NAME} \
//                             -t petalmd/auth3_xacte_net:${DOCKER_LABEL} \
//                             -f auth3.Dockerfile .
//                '''
//              }
//            }
//          }
//        }
//
//        stage('Build api_xacte_net') {
//          agent {
//            label buildLabel()
//          }
//          steps {
//            script {
//              checkout(scm).each { k,v -> env.setProperty(k, v) }
//              withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//                sh '''#!/bin/bash
//                set -e
//                docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                docker build --pull --no-cache \
//                             --build-arg BRANCH=${GIT_BRANCH} \
//                             --build-arg GIT_COMMIT=${GIT_COMMIT} \
//                             --build-arg JOB_NAME=${JOB_NAME} \
//                             --build-arg TAG_NAME=${TAG_NAME} \
//                             -t petalmd/api_xacte_net:${DOCKER_LABEL} \
//                             -f api.Dockerfile .
//                '''
//              }
//            }
//          }
//        }
//
//        stage('Push auth3_xacte_net') {
//          agent {
//            label buildLabel()
//          }
//          steps {
//            script {
//              withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//                sh '''#!/bin/bash
//                set -e
//                docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                docker push petalmd/auth3_xacte_net:${DOCKER_LABEL}
//                '''
//              }
//            }
//          }
//        }
//
//        stage('Push api_xacte_net') {
//          agent {
//            label buildLabel()
//          }
//          steps {
//            script {
//              withCredentials([usernamePassword(credentialsId: 'docker-hub-jenkins', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
//                sh '''#!/bin/bash
//                set -e
//                docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
//                docker push petalmd/api_xacte_net:${DOCKER_LABEL}
//                '''
//              }
//            }
//          }
//        }
//      }
//    }
//
//    stage('Docker push latest') {
//      agent {
//        label buildLabel()
//      }
//      when {
//        expression {
//          getBranchName() == 'master'
//        }
//      }
//      steps {
//        sh '''
//          docker tag petalmd/auth3_xacte_net:${DOCKER_LABEL} petalmd/auth3_xacte_net:latest
//          docker push petalmd/auth3_xacte_net:latest
//          docker tag petalmd/api_xacte_net:${DOCKER_LABEL} petalmd/api_xacte_net:latest
//          docker push petalmd/api_xacte_net:latest
//        '''
//      }
//    }
//    stage("Deploy STG1") {
//      when {
//        branch 'master'
//      }
//      agent {
//        label buildLabel()
//      }
//      steps {
//        script {
//          build(job: 'Xacte.v2 - Redeploy a staging environment', wait: true)
//        }
//      }
//    }
//  }
//
//  post {
//    success {
//      script {
//        unstash 'unit'
//        currentBuild.result = 'SUCCESS'
//        if (getBranchName() == "master") {
//          step([$class: 'MasterCoverageAction', scmVars: [GIT_URL: env.GIT_URL]])
//        } else {
//          step([$class: 'CompareCoverageAction', publishResultAs: 'comment', scmVars: [GIT_URL: env.GIT_URL]])
//        }
//        step([$class: 'CoberturaPublisher', coberturaReportFile: '**/cobertura.xml'])
//      }
//    }
//    always {
//      script {
//        try {
//          unstash 'unit'
//          sh 'echo "Archive artefacts"'
//          archiveArtifacts artifacts: 'reports/*.xml', fingerprint: true
//
//          sh 'echo "Validate test results"'
//          junit 'reports/*.xml'
//
//          if (getBranchName() == "master") {
//            def color = 'good'
//            def message = 'Build Succeeded'
//            if (currentBuild.result == 'FAILURE') {
//              color = 'danger'
//              message = 'Build Failed'
//            } else if (currentBuild.result == 'UNSTABLE') {
//              color = '#FFFE89'
//              message = 'Build Unstable'
//            }
//            slackSend channel: '#r_and_d_feed', color: color, message: "${env.JOB_NAME} - ${message} -  #${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
//          }
//        } finally {
//          // Cleanup images and containers
//          sh '''#!/bin/bash
//          docker system prune -af
//          '''
//
//          sh 'echo "Deprovision build environment"'
//          deprovisionBuildEnvironment()
//          deleteDir()
//        }
//      }
//    }
//  }
//}
//
//def getBranchName() {
//  branch = env.BRANCH_NAME
//  if (env.CHANGE_BRANCH) {
//      branch = env.CHANGE_BRANCH
//  }
//  return branch;
//}
//
//def escapeBranchName(branch) {
//  return branch.replaceAll("[^-_A-Za-z0-9]", "-");
//}
//
//def getDockerLabel() {
//  result = escapeBranchName(getBranchName())
//  if (result == "master") {
//      result = sh(returnStdout: true, script: 'date "+%Y-%m-%d_%H-%M"').trim()
//  }
//  currentBuild.description = "Docker tag : " + result
//  return result
//}
//
//def buildLabel() {
//  return "${env.JOB_NAME}".toLowerCase().replaceAll(~/[^-a-z0-9]/, '-').take(50) + "-${env.BUILD_NUMBER}"
//}
//
//def provisionBuildEnvironment() {
//  configFileProvider([configFile(fileId: 'xacte.api-gce', variable: 'GCE_JINJA')]) {
//    sh 'mkdir -p $(pwd) && cp "${GCE_JINJA}" gce.jinja'
//  }
//  sh '''
//    #!/bin/bash
//    ls .
//    docker pull petalmd/google-cloud-sdk 1> /dev/null
//    docker run --rm -i \
//      -v /ci/jenkins/gcloud:/root/.config/gcloud \
//      -v `pwd`:/config \
//      petalmd/google-cloud-sdk \
//        gcloud deployment-manager deployments create ${BUILD_LABEL} \
//          --template /config/gce.jinja \
//          --properties targetSize:${WORKERS},buildId:${BUILD_NUMBER},buildName:${BUILD_LABEL}
//  '''
//}
//
//def deprovisionBuildEnvironment() {
//  sh '''
//  docker run --rm -i \
//    -v /ci/jenkins/gcloud:/root/.config/gcloud \
//    petalmd/google-cloud-sdk \
//      gcloud deployment-manager deployments delete -q --async ${BUILD_LABEL}
//  '''
//
//  // Delete slave from Jenkins
//  script {
//    label = buildLabel()
//    for (aSlave in hudson.model.Hudson.getInstance().getSlaves()) {
//      if (aSlave.getLabelString() == label) {
//        aSlave.toComputer().doDoDelete();
//        println('Node ' + aSlave.getNodeName() + ' deleted');
//      }
//    }
//  }
//}
//