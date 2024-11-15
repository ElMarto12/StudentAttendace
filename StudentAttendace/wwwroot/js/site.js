let isLectureStarted = false;
let countdownInterval;

function toggleLecture() {
    const startButton = document.getElementById("startButton");
    const radioButtons = document.querySelectorAll(".attendance-radio");

    if (!isLectureStarted) {
        isLectureStarted = true;
        startButton.textContent = "Baigti";
        
        radioButtons.forEach(button => button.disabled = false);
        
        startCountdown(15);
    } else {
        isLectureStarted = false;
        startButton.textContent = "Pradėti";
        
        radioButtons.forEach(button => button.disabled = true);
        
        clearInterval(countdownInterval);
        document.getElementById("countdownTimer").textContent = "12:15";
    }
}

function startCountdown(minutes) {
    let time = minutes * 60;
    const countdownTimer = document.getElementById("countdownTimer");

    countdownInterval = setInterval(() => {
        const minutes = Math.floor(time / 60);
        const seconds = time % 60;
        countdownTimer.textContent = `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;

        if (time <= 0) {
            clearInterval(countdownInterval);
            countdownTimer.textContent = "Laikas baigėsi";
            toggleLecture();
        }

        time--;
    }, 1000);
}