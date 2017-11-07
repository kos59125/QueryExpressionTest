SELECT
   *
FROM
   repository r
WHERE
   r.owner_id = (SELECT account_id FROM account WHERE account_name = 'user1')
   OR r.owner_id IN (
      SELECT
         m.team_id
      FROM
         team_member m
      WHERE
         m.member_id = (SELECT account_id FROM account WHERE account_name = 'user1')
   )
;

SELECT
   r.*
FROM
   repository r
WHERE
   r.owner_id = (SELECT account_id FROM account WHERE account_name = 'user1')
   OR EXISTS (
      SELECT
         m.*
      FROM
         team_member m
      WHERE
         m.member_id = (SELECT account_id FROM account WHERE account_name = 'user1')
         AND m.team_id = r.owner_id
   )
;

SELECT
   *
FROM
   repository r
WHERE
   r.owner_id = (SELECT account_id FROM account WHERE account_name = 'user2')
   OR r.owner_id IN (
      SELECT
         m.team_id
      FROM
         team_member m
      WHERE
         m.member_id = (SELECT account_id FROM account WHERE account_name = 'user2')
   )
;

SELECT
   r.*
FROM
   repository r
WHERE
   r.owner_id = (SELECT account_id FROM account WHERE account_name = 'user2')
   OR EXISTS (
      SELECT
         m.*
      FROM
         team_member m
      WHERE
         m.member_id = (SELECT account_id FROM account WHERE account_name = 'user2')
         AND m.team_id = r.owner_id
   )
;
