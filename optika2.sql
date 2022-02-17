create database optika2 character set utf8;

use optika2;

create table haracteris(
id_har int(6) not null primary key auto_increment,
har_name char(20) not null) engine=InnoDB character set utf8;

create table categoria(
id_cat int(6) not null primary key auto_increment,
cat_name char(60) not null) engine=InnoDB character set utf8;

create table cater_harac(
id_ch int(6) not null primary key auto_increment,
id_cat int(6) not null,
id_har int(6) not null,
constraint ch_cat foreign key (id_cat) references categoria(id_cat) on delete cascade on update cascade,
constraint ch_har foreign key (id_har) references haracteris(id_har) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table posada(
id_pos int(6) not null primary key auto_increment,
posada char(20)not null) engine=InnoDB character set utf8;

create table spivrob(
sp_id int(6) not null primary key auto_increment,
sp_pib char(60) not null,
sp_nomer char(60) not null,
id_pos int(6) not null,
sp_status enum ('Активний','Звільнений') not null,
constraint sp_pos foreign key (id_pos) references posada(id_pos) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table spivroblog(
id_log int(6) not null primary key auto_increment,
sp_id int(6) not null,
login char(25) not null,
passwr char(25) not null,
constraint sp_log foreign key (sp_id) references spivrob(sp_id) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table tovar(
id_tov int(6) not null primary key auto_increment,
nazva char(100) not null,
id_cat int(6) not null,
price decimal(7,2) not null,
tovcount int(5) not null default 0, 
constraint tov_cat foreign key (id_cat) references categoria(id_cat) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table tovar_ch(
id_tc int(6) not null primary key auto_increment,
id_tov int(6) not null,
id_ch int(6) not null,
id_znach char(60) not null,
constraint tov_ch foreign key (id_tov) references tovar(id_tov) on delete cascade on update cascade,
constraint ch_tov foreign key (id_ch) references cater_harac(id_ch) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table clien(
cl_id int(6) not null primary key auto_increment,
cl_pib char(60) not null,
cl_nomer char(15) not null)
engine=InnoDB character set utf8;

create table clienbon(
cb_id int(6) not null primary key auto_increment,
cl_id int(6) not null,
bons int(6) not null default 0,
constraint bon_cl foreign key (cl_id) references clien(cl_id) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table zamovlenya(
id_zamov int(6) not null primary key auto_increment,
cl_id int(6) not null,
sp_id int(6) not null,
z_date DATE not null,
constraint cl_zm foreign key (cl_id) references clien(cl_id) on delete cascade on update cascade,
constraint sp_zm foreign key (sp_id) references spivrob(sp_id) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table tovarvzam(
id_tovzamov int(6) not null primary key auto_increment,
id_zamov int(6) not null,
id_tov int(6) not null,
kilkist int(6) not null,
constraint tv_zm foreign key (id_zamov) references zamovlenya(id_zamov) on delete cascade on update cascade,
constraint tz_tv foreign key (id_tov) references tovar(id_tov) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table postachal(
id_post int(6) not null primary key auto_increment,
name_postach char(50) not null)
engine=InnoDB character set utf8;

create table postach_tov(
id_pt int(6) not null primary key auto_increment,
id_post int(6) not null,
id_tov int(6) not null,
constraint pt_post foreign key (id_post) references postachal(id_post) on delete cascade on update cascade,
constraint pt_tov foreign key (id_tov) references tovar(id_tov) on delete cascade on update cascade)
engine=InnoDB character set utf8;

create table postavki(
id_postavki int(6) not null primary key auto_increment,
id_post int(6) not null,
id_tov int(6) not null,
tovcount int(5) not null,
constraint postavki_post foreign key (id_post) references postachal(id_post) on delete cascade on update cascade,
constraint postavki_tov foreign key (id_tov) references tovar(id_tov) on delete cascade on update cascade)
engine=InnoDB character set utf8;