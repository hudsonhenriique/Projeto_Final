const BASE_URL = 'http://localhost:5085/api/Clients';

// Elementos do DOM
const searchInput = document.getElementById('search-codigo');
const btnSearch = document.getElementById('btn-search');
const searchResult = document.getElementById('search-result');
const resultContent = document.querySelector('.result-content');

const editPanel = document.getElementById('edit-panel');
const editForm = document.getElementById('edit-form');
const editCodigo = document.getElementById('edit-codigo');
const editNome = document.getElementById('edit-nome');
const editTelefone = document.getElementById('edit-telefone');
const editEmail = document.getElementById('edit-email');
const btnCancel = document.getElementById('btn-cancel');

// Função para criar Toasts (Notificações flutuantes)
function showToast(message, type = 'success') {
    const container = document.getElementById('toast-container');
    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    
    const icon = type === 'success' 
        ? '<i class="fa-solid fa-circle-check"></i>' 
        : '<i class="fa-solid fa-circle-exclamation"></i>';
        
    toast.innerHTML = `${icon} <span>${message}</span>`;
    
    container.appendChild(toast);
    
    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100%)';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// Evento de Busca (Consulta)
btnSearch.addEventListener('click', async () => {
    const codigo = searchInput.value.trim();
    
    if (!codigo) {
        showToast('Por favor, informe o código do cliente.', 'error');
        return;
    }

    try {
        btnSearch.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> Buscando...';
        btnSearch.disabled = true;

        const response = await fetch(`${BASE_URL}/${codigo}`);
        
        if (!response.ok) {
            if (response.status === 404) {
                throw new Error('Cliente não encontrado no arquivo COBOL.');
            }
            throw new Error('Erro ao comunicar com a API.');
        }

        const data = await response.json();

        // Preenche o painel de edicao com os dados da API C#
        editCodigo.value = data.clientId;
        editNome.value = data.name;
        // Telefone e email mantem o nome em portugues apenas no front, mas lemos do ingles
        editTelefone.value = data.phone;
        editEmail.value = data.email;

        // Mostra mensagem de sucesso
        searchResult.className = 'result-box';
        resultContent.innerHTML = `<strong><i class="fa-solid fa-check"></i> Encontrado:</strong> ${data.name} (Cod: ${data.clientId})`;
        
        // Mostra painel de edição
        editPanel.classList.remove('hidden');
        showToast('Cliente carregado do Mainframe!');

    } catch (error) {
        searchResult.className = 'result-box error';
        resultContent.innerHTML = `<strong><i class="fa-solid fa-xmark"></i> Erro:</strong> ${error.message}`;
        editPanel.classList.add('hidden');
        showToast(error.message, 'error');
    } finally {
        btnSearch.innerHTML = '<i class="fa-solid fa-satellite-dish"></i> Buscar no Mainframe';
        btnSearch.disabled = false;
    }
});

// Permitir buscar apertando Enter
searchInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        e.preventDefault();
        btnSearch.click();
    }
});

// Evento de Cancelar
btnCancel.addEventListener('click', () => {
    editPanel.classList.add('hidden');
    searchInput.value = '';
    searchResult.classList.add('hidden');
    searchInput.focus();
});

// Evento de Salvar (Atualização)
editForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const codigo = editCodigo.value;
    const telefone = editTelefone.value;
    const email = editEmail.value;
    const btnSave = document.getElementById('btn-save');

    try {
        btnSave.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> Gravando...';
        btnSave.disabled = true;

        // A API C# espera os dados no corpo (body) com as propriedades exatas do Model ClientUpdateRequest
        const response = await fetch(BASE_URL, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ 
                clientId: codigo, 
                newPhone: telefone, 
                newEmail: email 
            })
        });

        if (!response.ok) {
            throw new Error('Falha ao atualizar dados no COBOL.');
        }

        showToast('Dados gravados no banco VSAM com sucesso!');
        
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btnSave.innerHTML = '<i class="fa-solid fa-floppy-disk"></i> Gravar Modificações';
        btnSave.disabled = false;
    }
});
