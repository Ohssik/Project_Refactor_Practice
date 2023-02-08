﻿const msgDiv = document.querySelector(".msgDiv"); //對話框
const userImg = document.querySelector("#userImg"); //客戶頭像
const txt = document.querySelector("#txt"); //輸入框
const btnSubmit = document.querySelector("#btnSubmit"); //送出
const tr = document.querySelectorAll("tr");
const resultDiv = document.querySelector("#resultDiv");
let azureLogo = document.querySelector("#azureLogo"); //按鈕
let logoDiv = document.querySelector("#logoDiv");
let timer = null; //時間變數

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
    //使用者頭像ajax
    const response = await fetch("/Problem/PUserImg");
    const img = await response.json();
    //問題ajax
    const request = await fetch(`/Problem/PAnswer?keyword=${txt.value}`);
    const data = await request.json();


    if (txt.value === "") {
        alert("請輸入內容")

        return false;
    }
    let fragForUser = document.createDocumentFragment();
    let fragForService = document.createDocumentFragment();

    fragForService = `<div class="display serviceDiv">
                     <div class="imgDiv"><img src="../images/Problem/dailyLogo.png" alt="ServiceImg" /></div>
                    <div class="serviceMsg">
                               ${data.answer}
                    </div>
                    </div>`;

    //Customer/Member
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

 let hight = document.documentElement.scrollHeight;  //需要顯示得總長
document.addEventListener("scroll", function () {
    clearTimeout(timer);

    let top = document.documentElement.scrollTop; //頁面目前所在高度
   
    const azureClass = azureLogo.classList.contains("walk");  //檢查是否有'walk'的class

    //停止捲動事件一秒後移除動畫class
    timer = setTimeout(function () {
        azureLogo.classList.remove("walk");
        logoDiv.classList.remove("walk");
    }, 500);

    //判斷有無此class
    if (!azureClass) {
        azureLogo.classList.add('walk');

        setTimeout(function () {
            logoDiv.classList.add("walk");
        }, 500)
    }

    
    //console.log("top:" + top);
    //console.log("hight:" + hight);
    //console.log(document.body.scrollHeight);
    //console.log("clientHeight: "+document.documentElement.clientHeight);
    //接近頁面最底時修改定位
    if ((hight - top) <= 1000) {
        azureLogo.setAttribute("style", "bottom:80px;");
        logoDiv.setAttribute("style", "bottom:80px");
        document.querySelector(".logo").setAttribute("style", "top:4%;")
        document.querySelector(".azure").setAttribute("style", "top:4%;")
    }
    else {
        azureLogo.setAttribute("style", "bottom:30px");
        logoDiv.setAttribute("style", "bottom:30px");
        document.querySelector(".logo").setAttribute("style", "top:8%;")
        document.querySelector(".azure").setAttribute("style", "top:8%;")
    }
})

//document.documentElement.clientHeight  目前瀏覽器顯示得高度
if (hight <= document.documentElement.clientHeight) {
    azureLogo.setAttribute("style", "bottom:80px;");
    logoDiv.setAttribute("style", "bottom:80px");
    document.querySelector(".logo").setAttribute("style", "top:4%;")
    document.querySelector(".azure").setAttribute("style", "top:4%;")
}