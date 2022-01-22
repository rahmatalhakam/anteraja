# **AnterAja**

This project is for AnterAja Backend and Microservice ++ !!! 
Layanan AnterAja merupakan layanan pemesanan ojek secara online. Layanan ini dapat memudahkan anda dalam menemukan ojek yang cepat dan murah.


## **Architecture** 

![architexture-pic](https://github.com/rahmatalhakam/anteraja/blob/main/case-study/mircoservice-architecture-anteraja.png)

Terdapat 4 service utama yang terdiri dari user service, admin service, driver service, dan transaction service. Database yang digunakan adalah satu sql server yang menampung 4 database dari 4 service yang berbeda. Api gateway menggunakan ingress-nginx. Message broker menggunakan kafka untuk menghandle order yang dilakukan oleh user dan akan di consume oleh driver service. 

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

Layanan AnterAja menyediakan beberapa service diantaranya user service, driver service, transaction service dan admin service untuk memudahkan anda dalam menggunakan layanan ini. Service ini didukung unggul dengan adanya fitur seperti mendapatkan driver terdekat sehingga user tidak terlalu lama menunggu order, dan adanya wallet untuk driver dan user dapat memudahkan transaksi cashless.

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
     Jika jarak > 5 km (atau menyesuaikan) maka driver tidak dapat melakukan accept order. Handle ini digunakan agar driver yang melakukan accept order, 
     lokasinya tidak terlalu jauh dengan lokasi user.
   - Melakukan request post ke transaction service dengan input berupa user id, lat start, long start, lat end, long end, area dari kafka, ditambah driver id. 
     Output berupa id transaction yang akan digunakan untuk finish order.
   - Jika ada area maka price sesuai dengan price pada area tersebut, jika area tidak dimasukkan maka price yang akan digunakan adalah base price.
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

Service ini ditujukan untuk pengguna, driver dan admin layanan anteraja. Di dalam service ini admin dapat melihat semua transaksi, melakukan perubahan harga, melihat keseluruhan data harga. Untuk user pada service ini dapat melihat transaksi, transaction fee, jarak, melakukan top up saldo, membuat akun user wallet, withdraw saldo serta melihat saldo terakhir pada wallet. Untuk driver pada service ini dapat melakukan post transaction, finish transaction, melihat transaksi, jarak, melakukan top up saldo, membuat akun user wallet, withdraw saldo serta melihat saldo terakhir pada wallet.

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
   - Ketika berhasil melakukan transaction, saldo user akan dikurangi dengan melakukan request put ke transaction/wallet withdraw dengan input user id, dan debit=billing.
   - Hanya dapat dilakukan oleh Driver.


2. Finish Transaction

   - Input berupa driver id dan transaction id.
   - Melakukan pengecekan transaction id dan driver id, jika kedua id tersebut tidak ditemukan, maka proses tersebut gagal.
   - set status otomatis menjadi finished=2
   - ketika berhasil melakukan finish trasaction, saldo driver akan bertambah dengan melakukan request put ke transaction/wallet top up dengan input berupa driver id dan credit=billing.
   - Hanya dapat dilakukan oleh Driver.

3. Get Transactions

   - Untuk mendapatkan list data seluruh transaksi berupa id transaksi, user id, driver id, lat dan long start, lat dan long end, created at, status, distance, billing, status order, dan price.
   - Hanya dapat dilakukan oleh Admin.

4. Get Transactions By UserId/DriverId

   - Melakukan pengecekan role berdasarkan token, jika token tersebut role as user, maka id yang diambil adalah id user, dan ditampilkan data user. 
   - Melakukan pengecekan role berdasarkan token, jika token tersebut role as driver, maka id yang diambil adalah id driver, dan ditampilkan data driver. 
   - Hanya dapat dilakukan oleh Driver dan User.

5. Update Price

   - Input berupa price id dan price.
   - Sebelum melakukan input price, dilakukan pengecekan price id terlebih dahulu,
     jika price id tersebut tidak ditemukan, proses update tersebut gagal.  
   - Hanya dapat dilakukan oleh Admin.

6. Get Prices

   - Untuk mendapatkan list data seluruh price.
   - Hanya dapat dilakukan oleh Admin.

7. Get Transaction Fee By User Id

   - Melakukan pengecekan 'order fee' dengan input user id, lat start, long end, lat start, long end, area.
   - Melakukan pengecekan user wallet apakah sudah terdaftar pada user wallet atau belum.
   - Melakukan pengecekan billing berdasarkan saldo user id, jika billing < saldo, 
     maka order dapat dilakukan dengan output info price, jika tidak maka user tidak dapat melakukan order.
   - Hanya dapat dilakukan oleh User. 
   

8. Get Distance

   - Untuk mendapatkan jarak dengan input lat dan long start, lat dan long end.

9.  Top Up Saldo

    - Input berupa credit dan id.
    - Melakukan pengecekan id apakah sudah terdaftar pada wallet user atau belum. Jika belum maka error, dan harus membuat user wallet terlebih dahulu.
    - Hanya dapat dilakukan oleh Driver dan User.

10. Add User Wallet
 
    - Input berupa user/driver id.
    - Melakukan pengecekan user id / driver id, jika user/driver id tersedia maka dapat mendaftar user wallet.
    - Melakukan pengecekan customer id, jika user/driver sudah terdaftar pada user wallet maka user/driver tidak dapat melakukan pendaftaran user wallet lagi.
    - Hanya dapat dilakukan oleh Driver dan User.

11. Withdraw Saldo

    - Input berupa debit dan id.
    - Melakukan pengecekan user id / driver id, jika user/driver id tersedia maka dapat melakukan withdraw saldo.
    - Melakukan pengecekan customer id, jika user/driver sudah terdaftar pada user wallet maka user/driver dapat melakukan withdraw saldo.
    - Hanya dapat dilakukan oleh Driver dan User.

12. Get Saldo By Id

    - Input berupa customer id.
    - Melakukan pengecekan customer id, jika customer id tersedia maka akan menampilkan saldo terakhir.
    - Hanya dapat dilakukan oleh Driver dan User.
