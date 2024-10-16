select sum(totalPrice)/ count(distinct(user_id)) AS ARPU
from transactions
join users
on player_id= user_id