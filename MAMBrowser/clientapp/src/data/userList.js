function RandomPick(data) {
    const value = data[Math.floor(Math.random() * data.length)];
    return value;
  }
  
 const total = 20;
 const disk = [
    { id: '10', name: "10gb" },
    { id: '20', name: "20gb" },
    { id: '30', name: "30gb" },
    { id: '40', name: "40gb" },
    { id: '50', name: "50gb" },
    { id: '60', name: "60gb" },
    { id: '70', name: "70gb" },
 ];

 const menuType = [
   { id: 'ALL', name: '전체 메뉴' },
   { id: 'menu1', name: '메뉴 유형1' },
   { id: 'menu2', name: '메뉴 유형2' },
 ];
 
 const mockData = {
    data: Array(total)
      .fill()
      .map((item, idx) => ({
        rowNO: idx + 1,
        name: `TEST ${idx}`,
        diskAllocation: RandomPick(disk),
        menuType: RandomPick(menuType),
      })),
  };
  
export default mockData;
