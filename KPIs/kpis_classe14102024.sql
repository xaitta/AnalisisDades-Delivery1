SELECT  date(sessions.start), count(distinct users.user_id) as DAU
from sessions, users
where user_id = player_id and users.country = 'China'
group by 1
order by 1;