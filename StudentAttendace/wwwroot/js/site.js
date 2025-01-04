let isLectureStarted = false;
let countdownInterval;

function toggleLecture() {
    const startButton = document.getElementById("startButton");
    const radioButtons = document.querySelectorAll(".attendance-radio");

    if (!isLectureStarted) {
        isLectureStarted = true;
        startButton.textContent = "Baigti";
        
        radioButtons.forEach(button => button.disabled = false);
        
        startCountdown(1, 30);
    } else {
        isLectureStarted = false;
        startButton.textContent = "Pradėti";
        
        radioButtons.forEach(button => button.disabled = true);
        
        clearInterval(countdownInterval);
        document.getElementById("countdownTimer").textContent = "1:30:00";
    }
}

function startCountdown(hours, minutes) {
    // Convert hours and minutes to total seconds
    let time = hours * 3600 + minutes * 60;
    const countdownTimer = document.getElementById("countdownTimer");

    // Start the countdown interval
    const countdownInterval = setInterval(() => {
        const remainingHours = Math.floor(time / 3600);
        const remainingMinutes = Math.floor((time % 3600) / 60);
        const remainingSeconds = time % 60;

        // Update the countdown timer text
        countdownTimer.textContent =
            `${String(remainingHours).padStart(2, '0')}:` +
            `${String(remainingMinutes).padStart(2, '0')}:` +
            `${String(remainingSeconds).padStart(2, '0')}`;

        // Stop the countdown when the time reaches zero
        if (time <= 0) {
            clearInterval(countdownInterval);
            countdownTimer.textContent = "Laikas baigėsi";
            toggleLecture();
        }

        time--;
    }, 1000);
}
