function RandomPick(data) {
   const value = data[Math.floor(Math.random() * data.length)];
   return value;
 }
 
const total = 500;
const genderType = ['F', 'M'];

const mockData = {
   data: Array(total)
     .fill()
     .map((item, idx) => ({
       id: idx + 1,
       name: `TEST NAME ${idx}`,
       nickname: `nick Name ${idx}`,
       email: `ontouchcancel.smitham@example.com ${idx}`,
       birthdate: '2020-07-12 00:00:00',
       gender: RandomPick(genderType),
     })),
 };
 


 export default mockData;
 