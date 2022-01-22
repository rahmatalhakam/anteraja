# **AnterAja**

This project is for AnterAja Backend and Microservice ++ !!! (add more explanation)

## **Architecture** (TODO: Rahmat)

![architexture-pic](update_later.jpg)

(brief explanation)

## **How to build docker file**

1. docker build -t OWNER/adminservice .
2. docker push OWNER/adminservice
3. docker build -t OWNER/driverservice .
4. docker push OWNER/driverservice
5. docker build -t OWNER/transactionservice .
6. docker push OWNER/transactionservice
7. docker build -t OWNER/userservice .
8. docker push OWNER/userservice

## **How to run on kubernetes**

```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install my-release bitnami/kafka
kubectl create secret generic mssql --from-literal=SA_PASSWORD="Kosongkan@Saja"
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

   - ketika login akan di cek status user terlebih dahulu,
     jika terblokir maka user tidak dapat login.
   - user dapat login dan mendapatkan token dengan role user.

3. Order

   - cek status user, jika blokir tidak bisa order
   - check lat dan long start dengan lat dan long end, jika jarak > 30 km (atau menyesuaikan) order ditola
   - check order fee ke transaction service, dengan input user id, lat start, long end, lat start, long end, area?
   - return dari check order fee berupa persetujuan bisa tidaknya melakukan order jika user id ditambahkan
   - input order berupa user id, lat start, long start, lat end, long end, area?

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
