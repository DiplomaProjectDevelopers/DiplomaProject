function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}

function chunkArray(array, chunk_size) {
    const arrayLength = array.length;
    const tempArray = [];

    for (let index = 0; index < arrayLength; index += chunk_size) {
        const myChunk = array.slice(index, index + chunk_size);
        tempArray.push(myChunk);
    }

    return tempArray;
}