CREATE TABLE account (
   account_id CHAR(36) NOT NULL,
   account_name VARCHAR(16) NOT NULL,
   PRIMARY KEY (account_id)
);

CREATE TABLE user (
   account_id CHAR(36) NOT NULL,
   PRIMARY KEY (account_id)
   FOREIGN KEY (account_id) REFERENCES account(account_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE team (
   account_id CHAR(36) NOT NULL,
   PRIMARY KEY (account_id)
   FOREIGN KEY (account_id) REFERENCES account(account_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE team_member (
   team_id CHAR(36) NOT NULL,
   member_id CHAR(36) NOT NULL,
   PRIMARY KEY (team_id, member_id),
   FOREIGN KEY (team_id) REFERENCES team(account_id) ON UPDATE CASCADE ON DELETE CASCADE,
   FOREIGN KEY (member_id) REFERENCES user(account_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE repository (
   repository_id CHAR(36) NOT NULL,
   repository_name VARCHAR(16) NOT NULL,
   owner_id CHAR(36) NOT NULL,
   visibility VARCHAR(10) NOT NULL,
   PRIMARY KEY (repository_id),
   FOREIGN KEY (owner_id) REFERENCES account(account_id) ON UPDATE CASCADE ON DELETE CASCADE
);
