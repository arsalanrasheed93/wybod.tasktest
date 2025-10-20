<template>
  <div class="w-full max-w-lg">
    <!-- Header -->
    <header class="p-4 pb-0 space-y-4 bg-white shadow-sm">
      <div class="flex items-center justify-between">
        <h3 class="text-lg font-semibold">Add Task</h3>
        <button
          type="button"
          @click="onCancel"
          aria-label="Close"
          class="text-gray-500 hover:text-gray-700 transition-colors duration-150 cursor-pointer"
        >
          ✕
        </button>
      </div>
      <hr class="border-t border-gray-300" />
    </header>    

    <form @submit.prevent="onSubmit" class="space-y-4 p-4 bg-white shadow-sm">
      <div>
        <label class="block text-sm font-medium text-gray-700">Title</label>
        <input
          v-model="title"
          type="text"
          required
          :class="['mt-1 block w-full rounded-md border-gray-300 shadow-sm transition-colors duration-150 focus:border-gray-500 focus:border-gray-500', titleError ? 'border-red-400' : '']"
          class="h-12 border border-gray-300 pl-2"
        />
      </div>

      <div>
        <label class="block text-sm font-medium text-gray-700">Description</label>
        <textarea
          v-model="description"
          rows="4"
          :maxlength="descMax"
          class="mt-1 block w-full rounded-md border border-gray-300 shadow-sm transition-colors duration-150 focus:border-gray-500 focus:border-gray-500 h-32 pl-2"
        ></textarea>
        <div class="flex justify-end text-xs text-gray-500 mt-1">
          <span>{{ description.length }} / {{ descMax }}</span>
        </div>
      </div>

      <div class="grid grid-cols-2 gap-4 mb-10">
        <div>
          <label class="block text-sm font-medium text-gray-700">Priority</label>
          <select
            v-model="priority"
            class="mt-1 block w-full rounded-md border border-gray-300 shadow-sm transition-colors duration-150 h-12 focus:border-gray-500 focus:border-gray-500"
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Due Date</label>
          <input
            v-model="completedAt"
            type="date"
            class="mt-1 pl-1 block w-full rounded-md border border-gray-300 shadow-sm transition-colors duration-150 h-12 focus:border-gray-500 focus:border-gray-500"
          />
        </div>
      </div>

      <hr class="border-t border-gray-300" />
      <div class="flex justify-end gap-2">
        <button
          type="button"
          @click="onCancel"
          class="px-3 py-1 rounded-md bg-gray-100 text-gray-700 hover:bg-gray-200 transition-colors duration-150 cursor-pointer"
        >
          Cancel
        </button>
        <button
          type="submit"
          :disabled="isSubmitting || !title.trim()"
          class="px-3 py-1 rounded-md bg-indigo-600 text-white hover:bg-indigo-700 active:bg-indigo-800 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-150 cursor-pointer"
        >
          <span v-if="isSubmitting">Adding…</span>
          <span v-else>Add Task</span>
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { TaskItem } from '@/lib/models/task-item';
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { useToast } from 'vue-toastification'

const toast = useToast()
const emit = defineEmits<{
  (e: 'add', task: TaskItem): void
  (e: 'close'): void
}>()

const title = ref('')
const description = ref('')
const priority = ref('Medium')
const completedAt = ref<string | null>(new Date().toISOString().substring(0, 10))
const isSubmitting = ref(false)
const descMax = 500
const touchedTitle = ref(false)

const titleError = computed(() => touchedTitle.value && !title.value.trim())

const reset = () => {
  title.value = ''
  description.value = ''
  priority.value = 'Medium'
  completedAt.value = null
  touchedTitle.value = false
}

const onSubmit = async () => {
  touchedTitle.value = true
  if (!title.value.trim()) return
  isSubmitting.value = true

  const newTask = {
    id: crypto?.randomUUID?.(),
    title: title.value.trim(),
    description: description.value.trim(),
    isCompleted: false,
    createdAt: new Date().toISOString(),
    priority: priority.value,
    completedAt: completedAt.value ? new Date(completedAt.value).toISOString() : null
  }

   try {
    // send to server
    const res = await fetch('/api/tasks', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(newTask)
    })

    if (!res.ok) {
       toast.error(`Failed to create task (${res.status})`)
    }
    else {
      const created = (await res.json()) as TaskItem

      // add to UI
      emit('add', newTask)
      toast.success('Task added')
    }
  } catch (err) {
    toast.error(`Failed to create task (${(err as Error).message})`)
  }
  
}

const onCancel = () => {
  reset()
  emit('close')
}

// close on Escape
const onKeyDown = (ev: KeyboardEvent) => {
  if (ev.key === 'Escape') {
    onCancel()
  }
}

onMounted(() => {
  window.addEventListener('keydown', onKeyDown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', onKeyDown)
})
</script>