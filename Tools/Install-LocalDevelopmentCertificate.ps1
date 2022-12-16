$existingCert = Get-ChildItem -Path Cert:\LocalMachine\Root | Where-Object { $_.FriendlyName -like 'Local Development' } | Select-Object -First 1
if (-not $existingCert) {
    # Create a self signed certificate with a 30 year expiration date. By default, it's one year.
    Write-Output "Creating Self-Certificate"

    $certificateExpirationDate = (Get-Date).AddYears(30)
    $certificate = New-SelfSignedCertificate -DnsName "*.xacte.local" -Type SSLServerAuthentication -CertStoreLocation cert:\LocalMachine\My -NotAfter $certificateExpirationDate -FriendlyName "Local Development"
    Write-Output "Adding Self-Certificate to Root"
    $store = get-item Cert:\LocalMachine\Root
    $store.Open("ReadWrite")
    $store.Add($certificate)
    $store.Close()
    Write-Output "Self-Certificate addedd to Root $certificate.Thumbprint"
}
else {
    Write-Output "Self-Certificate already exists in Root $existingCert.Thumbprint"
}
