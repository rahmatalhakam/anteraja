# **AnterAja**

This project is for AnterAja Backend and Microservice ++ !!! (add more explanation)

## **Architecture** (TODO: Rahmat)

![architexture-pic](update_later.jpg)

(brief explanation)

## **How to build docker file**

```bash
docker build -t OWNER/userservice .
docker build -t OWNER/driverservice .
docker build -t OWNER/adminservice .
docker build -t OWNER/transactionservice .
docker push OWNER/userservice
docker push OWNER/driverservice
docker push OWNER/adminservice
docker push OWNER/transactionservice
```

## **How to run on kubernetes**

```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install my-release bitnami/kafka
kubectl apply -f mssql-plat-depl.yaml
kubectl apply -f local-pvc.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.3/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml
kubectl apply -f adminservice-depl.yaml
kubectl apply -f userservice-depl.yaml
kubectl apply -f driverservice-depl.yaml
kubectl apply -f transactionservice-depl.yaml
```

## **Services**

(brief explanation)

### User Service

(brief explanation)

Features:

1. Register

   - otomatis menambahkan role sebagai user ✅

   - dst

2. Login

(detail explanation based on features)

3. Etc

### Driver Service

(brief explanation)

Features:

1. Register

   - otomatis menambahkan role sebagai user ✅

   - dst

2. Login

(detail explanation based on features)

3. Etc

### Admin Service

(brief explanation)

**Features:**

1. Register

   - otomatis menambahkan role sebagai user ✅

   - dst

2. Login

(detail explanation based on features)

3. Etc

### Transaction Service

(brief explanation)

**Features:**

1. Post Transaction

   - penjelasan ✅

   - dst
