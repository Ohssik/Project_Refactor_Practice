const msgDiv = document.querySelector(".msgDiv"); //對話框
const userImg = document.querySelector("#userImg"); //客戶頭像
const txt = document.querySelector("#txt"); //輸入框
const btnSubmit = document.querySelector("#btnSubmit"); //送出
const tr = document.querySelectorAll("tr");
const resultDiv = document.querySelector("#resultDiv");
const azureLogo = document.querySelector(".azureLogo"); //按鈕
const logoDiv = document.querySelector(".logoDiv");

let msg = "";
// 點選常見問題
tr.forEach(function (td) {
    td.addEventListener("click", function () {
        console.log(this.firstElementChild.innerHTML);
        msg = this.firstElementChild.innerHTML;
        txt.value = msg;
    });
});

//按下送出鈕
btnSubmit.addEventListener("click", async function () {
    

    //使用者頭像
    const response = await fetch("/Problem/PUserImg");
    const img = await response.json();
    //Customer/Member


    console.log(img);

    if (txt.value === "") {
        alert("請輸入內容")

        return false;
    }
    let fragForUser = document.createDocumentFragment();
    let fragForService = document.createDocumentFragment();

    const request = await fetch(`/Problem/PAnswer?keyword=${txt.value}`);
    const data = await request.json();
    //console.log(data)
    fragForService = `<div class="display serviceDiv">
                     <div class="imgDiv"><img src="../images/Problem/dailyLogo.png" alt="ServiceImg" /></div>
                    <div class="serviceMsg">
                               ${data.answer}
                    </div>
                    </div>`;

    //判斷使用者有無頭像
    if (img.userImg !== "") {
        fragForUser = `<div class="display userDiv">
                                    <div class="userMsg">${txt.value}</div>
                                        <div class="imgDiv" id='userImg'><img src="../images/Customer/Member/${img.userImg}" alt="SessionImg"/></div>
                                    </div > `;
    }
    else {
        fragForUser = `<div class="display userDiv">
                                    <div class="userMsg">${txt.value}</div>
                                        <div class="imgDiv" id='userImg'><img src="../images/Problem/user.png" alt="UserImg" /></div>
                                    </div > `;
    }
    resultDiv.innerHTML += fragForUser;
    resultDiv.innerHTML += fragForService;
    msgDiv.scrollTop = msgDiv.scrollHeight;

    txt.value = "";
});

document.addEventListener("scroll", function () {
    let top = document.documentElement.scrollTop; //頁面目前所在高度
    let hight = document.documentElement.scrollHeight;  //頁面總長
    console.log("top : "+top)
    console.log("top1 : " + hight)
    console.log(top % 10);
    if (top >= 1470) {
        azureLogo.setAttribute("style", "bottom:80px;");
        logoDiv.setAttribute("style", "bottom:80px");
    }
    else {
 azureLogo.setAttribute("style", "bottom:30px");
        logoDiv.setAttribute("style", "bottom:30px");
    }
    //if (top % 10 === 0) {
       
    //    //azureLogo.setAttribute("class", "step");
    //    //if (azureLogo.getElementsByClassName("step")) {

    //    //}
    //}
})
