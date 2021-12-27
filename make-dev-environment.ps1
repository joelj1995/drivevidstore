cd api
docker build --tag drivevidstoreapi:localdev . 

cd ..\worker
docker build --tag drivevidstoreworker:localdev . 

cd ..

docker compose up