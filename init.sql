CREATE TABLE cities
(
  Id SERIAL PRIMARY KEY,
  cityname CHARACTER VARYING(30),
  lougitude DECIMAL,
  latitude DECIMAL
);

CREATE TABLE history
(
  Id SERIAL PRIMARY KEY,
  datavalue date,
  timevalue time,
  cityname CHARACTER VARYING(30),
  weather INTEGER
);

