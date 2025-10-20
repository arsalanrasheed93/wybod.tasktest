<template>
  <div class="w-full max-w-lg bg-white rounded-md shadow-lg overflow-hidden">
    <header class="flex items-start justify-between p-4 border-b border-gray-100">
      <div>
        <h3 class="text-lg font-semibold">{{ task?.title ?? 'Task detail' }}</h3>
        <p class="text-xs text-gray-500 mt-1">Created: {{ formatDate(task?.createdAt) }}</p>
      </div>
      <button
        type="button"
        @click="close"
        aria-label="Close"
        class="text-gray-500 hover:text-gray-700 ml-4"
      >
        ✕
      </button>
    </header>

    <div class="p-4 space-y-4">
      <div>
        <label class="block text-xs font-medium text-gray-600">Description</label>
        <p class="mt-1 text-sm text-gray-800 whitespace-pre-wrap">
          {{ task?.description ?? '—' }}
        </p>
      </div>

      <div class="grid grid-cols-2 gap-4">
        <div>
          <label class="block text-xs font-medium text-gray-600">Priority</label>
          <div class="mt-1">
            <span
              class="inline-flex items-center px-2 py-1 rounded-full text-xs font-semibold"
              :class="priorityClass(task?.priority)"
            >
              {{ task?.priority ?? 'N/A' }}
            </span>
          </div>
        </div>

        <div>
          <label class="block text-xs font-medium text-gray-600">Due date</label>
          <div class="mt-1 text-sm text-gray-800">
            {{ task?.completedAt ? formatDate(task.completedAt) : '—' }}
          </div>
        </div>
      </div>

      <div class="grid grid-cols-2 gap-4">
        <div>
          <label class="block text-xs font-medium text-gray-600">Status</label>
          <div class="mt-1">
            <span
              :class="[
                'inline-flex items-center px-2 py-1 rounded-full text-xs font-medium',
                task?.isCompleted ? 'bg-green-100 text-green-800' : 'bg-amber-100 text-amber-800'
              ]"
            >
              {{ task?.isCompleted ? 'Completed' : 'Pending' }}
            </span>
          </div>
        </div>

        <div>
          <label class="block text-xs font-medium text-gray-600">Completed at</label>
          <div class="mt-1 text-sm text-gray-800">
            {{ task?.completedAt ? formatDate(task.completedAt) : '—' }}
          </div>
        </div>
      </div>
    </div>

    <footer class="flex justify-end gap-2 p-4 border-t border-gray-100 bg-gray-50">
      <button
        type="button"
        @click="close"
        class="px-3 py-1 rounded-md bg-white border border-gray-200 text-sm text-gray-700 hover:bg-gray-50"
      >
        Close
      </button>
    </footer>
  </div>
</template>

<script setup lang="ts">
import type { TaskItem } from '@/lib/models/task-item'

const props = defineProps<{
  task?: TaskItem | null
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const close = () => emit('close')

const formatDate = (iso?: string | null) => {
  if (!iso) return ''
  try {
    return new Date(iso).toLocaleString()
  } catch {
    return iso
  }
}

const priorityClass = (p?: string | null) => {
  switch (p) {
    case 'High':
      return 'bg-red-100 text-red-800'
    case 'Low':
      return 'bg-green-100 text-green-800'
    case 'Medium':
    default:
      return 'bg-amber-100 text-amber-800'
  }
}
</script>