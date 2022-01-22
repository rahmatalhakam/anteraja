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

Service ini ditujukan untuk pengguna layanan anteraja. Di dalam service ini pengguna dapat melakukan registrasi akun, login dan melakukan order. Service ini akan berhubungan dengan driver service dan transaction service.

Features:

1. Register

   - Melakukan registrasi.
   - User yang melakukan registrasi tersebut akan otomatis ditambahkan role sebagai user.

2. Login

   - Ketika login akan di cek status user terlebih dahulu, 
     jika terblokir maka user tidak dapat login.
   - User dapat login dan mendapatkan token dengan role user.

3. Order

   - Input 'order' berupa user id, lat start, long start, lat end, long end, area.
   - Melakukan pengecekan status user, jika status user tersebut "blokir" maka user tidak dapat order. 
   - Melakukan pengecekan lat dan long start dengan lat dan long end, 
     jika jarak > 30 km (atau menyesuaikan) maka order ditolak.
   - Melakukan pengecekan 'order fee' ke transaction service, dengan menginputkan user id, lat start, long end, lat start, long end, area. Output berupa persetujuan apakah user dapat melakukan order atau tidak jika user id ditambahkan.
   - Ketika order dilakukan, maka akan mengirim pesan ke kafka berupa data user id, lat start, long start, lat end, long end, area.
   - Hanya user dengan role 'User' yang dapat melakukan order.

4. Get Users
    
   - Untuk mendapatkan list data seluruh user berupa user id, username dan role.
   - Hanya dapat dilakukan oleh Admin.

5. Lock User

   - Untuk mengubah status user (enable or disable user).
   - Input berupa user id dan status (1 = untuk memblokir, 0 = untuk tidak memblokir) yang akan diberikan.
   - Output keterangan berhasil atau tidak berhasil mengubah status user.
   - Hanya dapat dilakukan oleh Admin.

6. Get User By Id
   - Input berupa user id. 
   - Melakukan pengecekan status user terlebih dahulu, 
     jika status user tersebut "blokir" maka user tidak dapat ditampilkan. 
   - Output berupa user id, username dan role user. 


### Driver Service

Service ini ditujukan untuk driver layanan anteraja. Di dalam service ini driver dapat melakukan registrasi akun, login dan melakukan set position dan accept order. Service ini akan berhubungan dengan user service dan transaction service. Service ini juga di set untuk melakukan consume kafka, sehingga mendapatkan data user id, lat start, long start, lat end, long end, area dan menyimpan hasil order dari user.

Features:

1. Register

   - Melakukan registrasi.
   - Driver yang melakukan registrasi tersebut akan otomatis ditambahkan role sebagai driver.
   - Status driver tersebut akan otomatis 'on progress' karena diperlukan persetujuan oleh admin terlebih dahulu.

2. Login

   - Ketika driver login akan di cek status driver terlebih dahulu, 
     jika terblokir maka driver tidak dapat login.
   - Driver dapat login dan mendapatkan token dengan role driver.

3. Get Driver Profile By Id
   
   - Input berupa driver id. 
   - Melakukan pengecekan status driver terlebih dahulu, 
     jika status driver tersebut "blokir" maka driver tidak dapat ditampilkan. 
   - Output berupa username driver.
   - Hanya dapat dilakukan oleh Driver.

4. Get Drivers

   - Untuk mendapatkan list data seluruh driver berupa username driver.
   - Hanya dapat dilakukan oleh Admin.

5. Lock User

   - Untuk mengubah status driver (enable or disable driver).
   - Input berupa driver id dan status (1 = untuk memblokir, 0 = untuk tidak memblokir) yang akan diberikan.
   - Output keterangan berhasil atau tidak berhasil mengubah status driver.
   - Hanya dapat dilakukan oleh Admin.

6. Set Position

   - Input berupa lat dan long driver.
   - Melakukan pengecekan status driver terlebih dahulu, 
     jika status driver tersebut "blokir" maka driver tidak dapat set position.
   - Hanya dapat dilakukan oleh Driver.

7. Accept Order

   - Input berupa driver id, lat dan long driver saat ini.
   - Melakukan pengecekan status driver terlebih dahulu, 
     jika status driver tersebut "blokir" maka driver tidak dapat accept order.
   - Melakukan pengecekan lat dan long driver dengan lat dan long start yang didapat dari hasil 'consume order kafka'. 
     Jika jarak > 5 km (atau menyesuaikan) maka driver tidak dapat melakukan accept order. Handle ini digunakan agar driver yang melakukan accept order, lokasinya tidak terlalu jauh dengan lokasi user.
   - Hanya dapat dilakukan oleh Driver.

8. Get Driver By Id
   - Input berupa driver id. 
   - Melakukan pengecekan status driver terlebih dahulu, 
     jika status driver tersebut "blokir" maka driver tidak dapat ditampilkan. 
   - Output berupa username driver.


### Admin Service

Service ini ditujukan untuk admin layanan anteraja. Di dalam service ini admin dapat melakukan registrasi akun, login agar mendapatkan token sebagai admin, sehingga token tersebut dapat digunakan di user service, driver service dan transaction service untuk mengatur beberapa features pada service tersebut.

**Features:**

1. Register

   - Melakukan registrasi.
   - Admin yang melakukan registrasi tersebut akan otomatis ditambahkan role sebagai admin.

2. Login

   - Admin dapat login dan mendapatkan token dengan role admin.

3. Get Admin By Id

   - Input berupa admin id. 
   - Melakukan pengecekan id terlebih dahulu, 
     jika id admin tersebut tidak tersedia maka admin tidak dapat ditampilkan. 
   - Output berupa admin id, username dan role admin. 
   - Hanya dapat dilakukan oleh Admin.

4. Get Admins

   - Untuk mendapatkan list data seluruh admin berupa id, username dan role admin.
   - Hanya dapat dilakukan oleh Admin.

### Transaction Service

(brief explanation)

**Features:**

1. Post Transaction

   - Input berupa user id, driver id, lat start, long start, lat end, long end, area.
   - Melakukan pengecekan user id, jika user id tidak ditemukan maka transaksi tersebut gagal.
   - Melakukan pengecekan driver id, jika driver id tidak ditemukan maka transaksi tersebut gagal.
   - Ketika transaksi berhasil dilakukan, set status order menjadi 'on progress=1' dan set 'created' pada tanggal saat transaksi tersebut dilakukan.
   - Melakukan pengecekan jarak lat long antara start dengan end, 
     jika jarak > 30 km, maka tidak dapat melakukan transaction. 
   - Melakukan pengecekan field area, jika ada areanya maka set price berdasarkan area, 
     jika area tersebut tidak ada maka set base price.
   - Set billing berdasarkan price id dikali dengan jarak yang ditempuh. 
   - Ketika berhasil melakukan transaction, saldo user akan dikurangi dengan request put ke transaction/wallet withdraw dengan input user id, dan debit=billing.
   - Hanya dapat dilakukan oleh Driver.
