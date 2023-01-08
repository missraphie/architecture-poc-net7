# add certificate to Personal, Trusted root and Trusted publishers stores

$existingCert = Get-ChildItem -Path Cert:\LocalMachine\Root | Where-Object { $_.FriendlyName -like 'Local Development' } | Select-Object -First 1
if (-not $existingCert) {
    #create a SAN cert for both host.docker.internal, gateway.docker.internal and localhost
    Write-Output "Creating Self-Certificate"
    $certificateExpirationDate = (Get-Date).AddYears(30)
    $cert = New-SelfSignedCertificate -DnsName "xacte.local", "*.xacte.local", "host.docker.internal", "gateway.docker.internal", "localhost" -CertStoreLocation cert:\localmachine\my -NotAfter $certificateExpirationDate -FriendlyName "Local Development"

    #export it for docker container to pick up later
    $password = ConvertTo-SecureString -String "xacte4ever" -Force -AsPlainText
    Export-PfxCertificate -Cert $cert -FilePath $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -Password $password

    # trust it on your host machine
    Write-Output "Adding Self-Certificate to Trusted Publisher + Local Machine"
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store "TrustedPublisher","LocalMachine"
    $store.Open("ReadWrite")
    $store.Add($cert)
    $store.Close()

    Write-Output "Adding Self-Certificate to Root"
    $store = get-item Cert:\LocalMachine\Root
    $store.Open("ReadWrite")
    $store.Add($cert)
    $store.Close()
    Write-Output "Self-Certificate addedd to Root $certificate.Thumbprint"
}
else {
    Write-Output "Self-Certificate already exists in Root $existingCert.Thumbprint"
}