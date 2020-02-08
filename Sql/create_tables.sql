create table JOUEUR
(
    login_id varchar(128),
    passwrd varchar(128),
    email varchar(512),
    nbr_parties integer default 0, --nbr parties total
    nbr_parties_g integer default 0, --nbr parties gagnees
    nbr_parties_p integer default 0, --nbr parties perdues
    date_insc date default CURRENT_DATE NOT NULL, --date d'inscription

    constraint PK_joueurs 
        PRIMARY KEY(login_id, passwrd),
    constraint U_joueurs
        UNIQUE(email)
);

----------------------------------

create table PERSONNAGE
(
    nom varchar(128),
    faction varchar(128) NOT NULL, --shadow/neutre/hunter
    pv integer NOT NULL, --points de vie
    cond_vict varchar(512) NOT NULL, --conditions de victoire
    capacite varchar(512) NOT NULL, --capacite speciale

    constraint PK_personnage
        PRIMARY KEY(nom)
);

----------------------------------

create table LIEU
(
    nom varchar(128),
    effet varchar(128) NOT NULL,

    constraint PK_lieu
        PRIMARY KEY(nom)
);

----------------------------------

create table CARTES_EFFET --cartes à jouer immediatement
(
    nom varchar(128),
    type_carte varchar(128) NOT NULL, --vision/lumiere/tenebres
    effet varchar(512) NOT NULL,

    constraint PK_cartes_effet
        PRIMARY KEY(nom)
);

----------------------------------

create table AMIS
(
    login_id1 varchar(128),
    login_id2 varchar(128), 

    constraint PK_amis
        PRIMARY KEY(login_id1, login_id2),
    constraint FK_amis1
        FOREIGN KEY(login_id1) references JOUEUR on delete cascade,
    constraint FK_amis2
        FOREIGN KEY(login_id2) references JOUEUR on delete cascade
    --si on supprime le joueur de la table "JOUEUR", toutes les 
    --occurrences ou il apparait dans la table "AMIS" seront 
    --supprimées aussi
);

