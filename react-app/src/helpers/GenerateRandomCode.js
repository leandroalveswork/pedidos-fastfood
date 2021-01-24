const randomHex = () => {
    const randomDec = Math.floor(Math.random()*16);
    return randomDec < 10 ? randomDec.toString() : String.fromCharCode('a'.charCodeAt(0) + randomDec - 10)
}

export default function generateRandomCode() {
    let code = ''
    for (let i = 0; i < 5; i++)
    {
        for (let j = 0; j < 4; j++)
        {
            code += randomHex();
        }
        code += i < 4 ? '-' : ''
    }
    return code;
}