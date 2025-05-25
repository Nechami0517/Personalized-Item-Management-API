
let authors = [];
let books = [];
let token = getCookie('authToken');
let userRole;
let userId;
let userName;


const getAuthorsAndBooksList = async () => {
    try {
        const [authorsResponse, booksResponse] = await Promise.all([
            axios.get(`/Author`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }),
            axios.get(`/Book`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
        ]);

        authors = authorsResponse.data; // שמירה של רשימת הסופרים במשתנה הגלובלי
        books = booksResponse.data; // שמירה של רשימת הספרים במשתנה הגלובלי

    
    } catch (error) {
        console.error("Error fetching data", error);
    }
};
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}

function getUserRoleFromToken() {
    const token = getCookie('authToken');
    if (!token) {
        return null;
    }

    const payLoad = token.split('.')[1];
    const deCodedPayLoad = JSON.parse(atob(payLoad));
    let roleValue = null;
    for (let key in deCodedPayLoad){
        if(key.includes("role")){
            roleValue = deCodedPayLoad[key];
        }
        if(key.includes("id")){
            userId = deCodedPayLoad[key];
        }
        if(key.includes("name")){
            userName = deCodedPayLoad[key];
            userName = decodeUnicode(userName);
        }
        
    }

    return roleValue;
}
 decodeUnicode = (str) =>{
return str.replace(/\\u([\dA-Fa-f]{4})/g, function (match, grp) {
        return String.fromCharCode(parseInt(grp, 16));
    });
}




