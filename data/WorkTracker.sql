CREATE TABLE "Groups" ("GroupId" INTEGER PRIMARY KEY  NOT NULL ,"Name" VARCHAR(32),"Description" varchar(160));
CREATE TABLE "Projects" ("ProjectId" INTEGER PRIMARY KEY  NOT NULL ,"Name" VARCHAR(32),"Description" Varchar(160),"GroupId" int);
CREATE TABLE "WorkTracks" ("WorkTrackId" INTEGER PRIMARY KEY  NOT NULL ,"Created" DATETIME DEFAULT (CURRENT_TIMESTAMP) ,"Duration" DOUBLE NOT NULL ,"FromDate" DATETIME,"ToDate" DATETIME,"ProjectId" INTEGER,"Notes" VARCHAR(255),"Location" VARCHAR(120), "Subject" varchar(50));
