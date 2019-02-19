FROM mysql:5.7

ENV MYSQL_ROOT_PASSWORD="Password*"
ENV MYSQL_DATABASE="mydb"

ADD mydbExtended.sql /docker-entrypoint-initdb.d/mydbExtended.sql
ADD PopulateExtended.sql /docker-entrypoint-initdb.d/PopulateExtended.sql

RUN chown -R mysql:mysql /docker-entrypoint-initdb.d/