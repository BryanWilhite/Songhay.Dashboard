#!/bin/bash

echo "deploying Dashboard server...";

(cd ./Songhay.Dashboard.Server/bin/Release/net6.0/publish && zip -r ~/appRoot/songhay/dashboard-test.zip .)

az webapp deploy \
    --resource-group songhay-system-resources-free \
    --name songhay-system-staging \
    --src-path ~/appRoot/songhay/dashboard-test.zip \
    --type zip --clean true

exit 0
