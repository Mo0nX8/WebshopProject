async function sendMessage() {
    const input = document.getElementById("userInput").value;
    if (!input) return;

    const messages = document.getElementById("messages");
    messages.innerHTML += `<div class="user-msg" style="color:blue;margin-bottom:5px;">You: ${input}</div>`;

    const response = await fetch('/api/chatbot/ask', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ question: input })
    });
    const data = await response.json();

    messages.innerHTML += `<div class="bot-msg" style="color:green;margin-bottom:5px;">Bot: ${data.answer}</div>`;
    messages.scrollTop = messages.scrollHeight; 
    document.getElementById("userInput").value = "";
}
