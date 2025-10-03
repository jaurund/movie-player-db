const countContent = document.getElementById("count-content");
const getContent = document.getElementById("get-content");
const videoPlayer = document.getElementById("video-player");
const likeButton = document.getElementById("like-button");
let isLiked = false;

function openSidepanel() {
    document.getElementById("my-sidepanel").style.width = "150px";
    document.getElementById("open-sidepanel-btn").style.display = "none";
    getContent.style.display = "none";
    document.getElementById("media-content").style.display = "flex";
    videoPlayer.style.display = "block";
}

function closeSidepanel() {
    document.getElementById("my-sidepanel").style.width = "0";
    document.getElementById("my-sidepanel").style.transition = "1s";
    document.getElementById("open-sidepanel-btn").style.display = "block";
    getContent.style.display = "block";
    document.getElementById("media-content").style.display = "none";
    videoPlayer.style.display = "none";
    ifSidePanelClosed();
}
/**
 * warning: this method has several bugs, it is unable to send send data to the DTO.
 * @param {*} id video-id
 * @param {*} numberOfLikes how many likes does the video have? 
 * @param {*} url RestAPI Endpoint that supports the HTTP-POST method
 */
async function sendVideoLike(id, numberOfLikes, url) {
    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ "id": id, "numberOfLikes": numberOfLikes })
        })
    }
    catch {
        console.error("An error occured!");
    }
}

likeButton.addEventListener("click", (id, numberOfLikes) => {
    console.log("clicked..");
    console.log(isLiked);
    id = 1;
    numberOfLikes = 0;
    if (!isLiked) {
        id++;
        numberOfLikes++;
        sendVideoLike(id, numberOfLikes, `http://localhost:5067/api/videos/${id}/like`);
        likeButton.src = "./src/icons/like_blue.svg";
        console.log(isLiked);
    }
    else {
        likeButton.src = "./src/icons/like.svg";
        numberOfLikes--;
    }
    isLiked = !isLiked;
    console.log(isLiked);
})

/**
 * GET endpoint
 * @returns JSON object data from the RestAPI
 */
async function GetVideoData(url) {
    try {
        const response = await fetch(url);
        const data = await response.json();
        return data.map(value => {
            value.id,
                value.title,
                value.description,
                value.videoURL,
                value.numberOfLikes
        });
    }
    catch {
        console.error("There was an error fetching the data from the server!");
    }
}


document.addEventListener("DOMContentLoaded", () => {
    const videoDescriptionElement = document.getElementById("video-description");
    const videoMetadata = GetVideoData("http://localhost:5067/api/videos/");

    /*async function fetchVideoFeed(url) {
        try {
            const response = await fetch(url);
            const videoObject = await response.json();

            if (videoObject.length > 0) {
                for (let i = 0; i < videoObject.length; i++) {
                    console.log(videoObject);
                    console.log("video url:", videoObject[i].videoURL);
                    console.log("video description:", videoObject[i].description)
                    console.log("video likes:", videoObject[i].numberOfLikes);
                }

                videoPlayer.src = videoObject[0].videoURL;
                videoPlayer.volume = 0.3;
                await videoPlayer.play();

                videoDescriptionElement.textContent = videoObject[0].description;
            }

            window.ifSidePanelClosed = () => {
                videoPlayer.pause();
                closeSidepanel();
            }
        }
        catch (error) {
            console.error(`Error fetching video feed: ${error}`);
        }
    }*/
    // driver code with arguments!
    const videoFeedURL = "https://amd-vgtv.akamaized.net/vgtv/vod/2025/04/68107d146802ce00080198aa/540_2000_pkg.mp4";
    fetchVideoFeed(videoFeedURL);

    fetch('http://localhost:5067/api/videos')
        .then(response => response.json())
        .then(videos => {
            const video = videos[0]; // Uses the first video in the list
            document.getElementById('video-player').src = video.videoURL;
            document.getElementById('video-title').textContent = video.title;
            document.getElementById('video-description').textContent = video.description;
        });
})

window.ifSidePanelClosed = function() {
    videoPlayer.pause();
    closeSidepanel();
};
