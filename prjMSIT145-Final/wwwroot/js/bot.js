﻿
    const msgDiv = document.querySelector(".msgDiv"); //對話框
    const userImg = document.querySelector("#userImg"); //客戶頭像
    const txt = document.querySelector("#txt"); //輸入框
    const btnSubmit = document.querySelector("#btnSubmit"); //送出
    const tr = document.querySelectorAll("tr");
    const resultDiv = document.querySelector("#resultDiv");

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
            if (txt.value === "") {
        alert("請輸入內容")

                return false;
            }
    let fragForUser = document.createDocumentFragment();
    let fragForService = document.createDocumentFragment();

    const request = await fetch(`@Url.Content("~/Problem/PAnswer?keyword=${txt.value}")`);
    const data = await request.json();
    console.log(data)
    fragForService = `<div class="display serviceDiv">
        <div class="imgDiv"><img src="@Url.Content("~/images/Problem/dailyLogo.png")" alt="ServiceImg" /></div>
        <div class="serviceMsg">
            ${data.answer}
        </div>
    </div>
</div>`;

            fragForUser = `< div class="display userDiv" >
                                    <div class="userMsg">${txt.value}</div>
                                        <div class="imgDiv"><img src="@Url.Content("~/images/Problem/user.png")" alt="UserImg" /></div>
                                    </div > `;

            resultDiv.innerHTML += fragForUser;
            resultDiv.innerHTML += fragForService;
            msgDiv.scrollTop = msgDiv.scrollHeight;

            txt.value = "";
    });
