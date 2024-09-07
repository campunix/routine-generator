// Get the number of rows and columns
const rows = 10;
const columns = 5;

// Get the container where the table will be displayed
const tableContainer = document.getElementById('tableContainer');

// Clear any existing table
tableContainer.innerHTML = '';

// Create a new table element
const table = document.createElement('table');
table.style.borderCollapse = 'collapse'; // Make sure borders are collapsed

var cellIdCounter = 0;
// Generate rows and columns
for (let i = 0; i < rows; i++) {
    const tr = document.createElement('tr');

    for (let j = 0; j < columns; j++) {
        const td = document.createElement('td');
        // td.textContent = `cell-${cellIdCounter}`; // Example content
        td.style.border = '1px solid black'; // Set border for table cells
        td.style.padding = '8px'; // Add some padding
        td.style.height = '40px';
        td.style.width = '120px';
        td.id = `cell-${cellIdCounter}`;
        tr.appendChild(td);

        cellIdCounter++
    }

    table.appendChild(tr);
}

// Append the table to the container
tableContainer.appendChild(table);

populateData();

async function populateData() {
    var data = [];

    const apiUrl = 'https://localhost:7292/routine';
    let apiData = [];
    try {
        const response = await fetch(apiUrl);
        apiData = await response.json();
        data = apiData.genes || [];
    } catch (error) {
        console.error('Error fetching data from API:', error);
        return;
    }
    data.forEach(item => {
        var element = document.getElementById(`cell-${item.cellNumber}`);
        if (element)
        {
            element.textContent = `${item.courseCode} (${item.courseTeacher})`;
        }
    });
}